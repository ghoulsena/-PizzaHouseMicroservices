using Microsoft.AspNetCore.Authorization;

namespace OrderService.Presentation.OrderOwnerRequirement;

public class OrderOwnerRequirement : IAuthorizationRequirement { }
// IAuthorizationRequirement это лишь макетный интерфейс который говорит что  этот обьект требования к авторизации,он всего лишь как ярлык
//Основная служба, которая определяет успешность авторизации:IAuthorizationService
//IAuthorizationRequirement — это служба маркеров без методов и механизм отслеживания успешности авторизации.