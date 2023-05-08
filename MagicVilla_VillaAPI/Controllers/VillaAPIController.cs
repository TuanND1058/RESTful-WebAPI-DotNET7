using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;

        public VillaAPIController(IVillaRepository villaRepository, IMapper mapper)
        {
            _villaRepository = villaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillasAsync()
        {
            IEnumerable<Villa> villaList = await _villaRepository.GetAllAsync();

            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVillaAsync(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Villa villa = await _villaRepository.GetAsync(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVillaAsync([FromBody] VillaCreateDTO createDTO)
        {
            /// neu mot controller duoc danh dau [ApiController]
            /// khong can kiem tra ModelState.IsValid o day
            /// ASP.NET da tu dong kiem tra va tra ve phan hoi BadRequest neu mo hinh khong hop le
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (await _villaRepository.GetAsync(x => x.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomErrorName", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            var model = _mapper.Map<Villa>(createDTO);
            model.CreatedDate = DateTime.Now;

            await _villaRepository.CreateAsync(model);

            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVillaAsync(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            Villa villa = await _villaRepository.GetAsync(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            await _villaRepository.RemoveAsync(villa);

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVillaAsync(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }

            var model = _mapper.Map<Villa>(updateDTO);

            await _villaRepository.UpdateAsync(model);

            return NoContent();
        }

        [HttpPatch("{id}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVillaAsync(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            /// JsonPatchOperations
            /// -operationType: Đây là thuộc tính kiểu enum OperationType xác định loại thao tác cần thực hiện trên đối tượng.
            ///     Các giá trị có thể có của OperationType bao gồm: Add (Thêm), Remove (Xóa), Replace (Thay thế), Move (Di chuyển), Copy (Sao chép), Test (Kiểm tra).
            /// -path: Đây là thuộc tính kiểu chuỗi xác định đường dẫn tới thuộc tính cần áp dụng thao tác. Ví dụ: "/Name" hoặc "/Address/City".
            /// -op: Đây là thuộc tính kiểu chuỗi xác định tên của thao tác cần áp dụng.Tên này giống với giá trị của enum OperationType,
            ///     ví dụ: "add", "remove", "replace", "move", "copy", hoặc "test".
            /// -from: Đây là thuộc tính kiểu chuỗi xác định đường dẫn tới thuộc tính nguồn(chỉ dùng cho thao tác Move và Copy).
            /// -value: Đây là thuộc tính kiểu object xác định giá trị mới của thuộc tính sau khi thực hiện thao tác. Thuộc tính này chỉ được sử dụng cho các thao tác Add, Replace và Test

            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            Villa villa = await _villaRepository.GetAsync(x => x.Id == id, false);

            if (villa == null)
            {
                return BadRequest();
            }

            var villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _mapper.Map<Villa>(villaDTO);

            await _villaRepository.UpdateAsync(model);

            return NoContent();
        }
    }
}
