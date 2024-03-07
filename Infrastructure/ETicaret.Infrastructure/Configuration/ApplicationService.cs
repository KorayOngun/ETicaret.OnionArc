using ETicaret.Application.Abstractions.Services.Configurations;
using ETicaret.Application.CustomAttribute;
using ETicaret.Application.DTOs.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Configuration
{
    public class ApplicationService : IApplicationService
    {

        public List<Menu> GetAuthorizeDefinitionEndPoints(Type assemblyType)
        {
            Assembly assembly = Assembly.GetAssembly(assemblyType);
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            if (controllers.Count() == 0) return default;

            List<Menu> menus = new List<Menu>();

            foreach (var controller in controllers) 
            {
                var actions = controller.GetMethods().Where(mi => mi.IsDefined(typeof(AuthorizeDefinitionAttribute)));
                if (actions.Count() > 0)
                {
                    foreach (var action in actions)
                    {
                        var attributes = action.GetCustomAttributes(true);
                        if (attributes.Count() > 0)
                        {
                            Menu menu;
                            var authorizeDefinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                            if (!menus.Any(m => m.Name == authorizeDefinitionAttribute.Menu))
                            {
                                menu = new Menu() {Name = authorizeDefinitionAttribute.Menu };
                                menus.Add(menu);
                            }
                            else
                            {
                                menu = menus.FirstOrDefault(m => m.Name == authorizeDefinitionAttribute.Menu);
                            }

                            Application.DTOs.Configuration.Action _action = new()
                            {
                                ActionType = Enum.GetName(authorizeDefinitionAttribute.ActionType),
                                Definition = authorizeDefinitionAttribute.Definition,
                            };

                            var http = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                            if (http != null)
                            {
                                _action.HttpType = http.HttpMethods.FirstOrDefault();
                            }
                            else
                            {
                                _action.HttpType = HttpMethods.Get;
                            }

                            _action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ","")}";

                            menu.Actions.Add(_action);  

                        }
                    }
                }
            }
            return menus;
        }
    }
}
