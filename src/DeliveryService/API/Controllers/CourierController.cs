using AutoMapper;
using DeliveryService.Application.DTOs;
using DeliveryService.Application.Service;
using DeliveryService.Domian.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly CourierService _courierService;
        private readonly IMapper _mapper;

        public CourierController(CourierService courierService, IMapper mapper)
        {
            _courierService = courierService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourierDto>>> GetAllCouriersAsync()
        {
            var couriers = await _courierService.GetAllCouriersAsync();
            var dtos = _mapper.Map<IEnumerable<CourierDto>>(couriers);
            return Ok(dtos);
        }


        [HttpGet("{id:guid}", Name = nameof(GetCourierByIdAsync))]
        public async Task<ActionResult<CourierDto>> GetCourierByIdAsync(Guid id)
        {
            var courier = await _courierService.GetCourierByIdAsync(id);
            if (courier == null)
                return NotFound(new { Message = "Courier not found" });

            return Ok(ToDto(courier));
        }


        [HttpPost]
        public async Task<ActionResult<CourierDto>> AddCourierAsync([FromBody] CourierDto courierDto)
        {
            var courier = FromDto(courierDto);
            if(courier == null)
                return BadRequest(new { Message = "Invalid courier data" });
            await _courierService.AddCourierAsync(courier.Name);

            return CreatedAtAction(
                nameof(GetCourierByIdAsync),
                new { id = courier.Id },
                ToDto(courier)
            );
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateCourierAsync(Guid id, [FromBody] CourierDto courierDto)
        {
            var courier = FromDto(courierDto);
            if (id != courier.Id)
                return BadRequest(new { Message = "Id mismatch" });

            await _courierService.UpdateCourierAsync(courier);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCourierAsync(Guid id)
        {
            var courier = await _courierService.GetCourierByIdAsync(id);
            if (courier == null)
                return NotFound(new { Message = "Courier not found" });

            await _courierService.DeleteCourierAsync(id);
            return NoContent();
        }
        private CourierDto ToDto(Courier courier) => _mapper.Map<CourierDto>(courier);
        private Courier FromDto(CourierDto dto) => _mapper.Map<Courier>(dto);
    }
}
