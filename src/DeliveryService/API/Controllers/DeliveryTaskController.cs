using AutoMapper;
using DeliveryService.Application.DTOs;
using DeliveryService.Application.Service;
using DeliveryService.Domian.Entity;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;


namespace DeliveryService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryTaskController : ControllerBase
    {
        private readonly DeliveryTaskService _deliveryTaskService;
        private readonly IMapper _mapper;
       

        public DeliveryTaskController(DeliveryTaskService deliveryTaskService, IMapper mapper)
        {
            _deliveryTaskService = deliveryTaskService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryTask>>> GetAllTasksAsync()
        {
            var tasks = await _deliveryTaskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id:guid}",Name = nameof(GetTaskByIdAsync))]
        public async Task<ActionResult<DeliveryTask>> GetTaskByIdAsync(Guid id)
        {
            var task = await _deliveryTaskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("ByOrder/{orderId:guid}")]
        public async Task<ActionResult<DeliveryTask>> GetTaskByOrderIdAsync(Guid orderId)
        {
            var task = await _deliveryTaskService.GetTaskByOrderIdAsync(orderId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult> AddTaskAsync([FromBody] DeliveryTaskDto taskdto)
        {
            var taskmap = _mapper.Map<DeliveryTask>(taskdto);
            var task = await _deliveryTaskService.CreateDeliveryTaskAsync(taskdto.OrderId, taskdto.CourierId);
            var taskresult = _mapper.Map<DeliveryTaskDto>(task);
            return CreatedAtAction("GetTaskById", new { id = task.Id }, taskresult);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateTaskAsync(Guid id, [FromBody] DeliveryTaskDto taskdto)
        {
            var task = _mapper.Map<DeliveryTask>(taskdto);
            if (id != task.Id)
                return BadRequest("Id mismatch.");

            await _deliveryTaskService.UpdateTaskAsync(task);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteTaskAsync(Guid id)
        {
            await _deliveryTaskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}
