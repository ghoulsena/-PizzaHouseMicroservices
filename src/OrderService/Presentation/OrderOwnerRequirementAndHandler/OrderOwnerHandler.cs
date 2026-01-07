using Microsoft.AspNetCore.Authorization;
using OrderService.Domain.Entities;

namespace OrderService.Presentation.OrderOwnerRequirement;
//Класс AuthorizationHandlerContext — это то, что обработчик использует для пометки соответствия требованиям:
public class OrderOwnHandler : AuthorizationHandler<OrderOwnerRequirement, Order>//в <> указываем требование которое необходимо обработать
{
    protected override Task HandleRequirementAsync(
         AuthorizationHandlerContext context,//предоставляем контекст, чтобы определить разрешен ли доступ
         //context — это объект AuthorizationHandlerContext, он хранит результаты проверки требований.
         OrderOwnerRequirement requirement,
        Order resource)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId != null && Guid.TryParse(userId, out var userGuid) && resource.UserId == userGuid)
        {
            context.Succeed(requirement);
            //когда мы вызываем Succeed(requirement) это значит что пользователь прошел проверку
            //fail не прошел
            //если ничего значит проверка не выполнена
        }
        return Task.CompletedTask;
    }
}
