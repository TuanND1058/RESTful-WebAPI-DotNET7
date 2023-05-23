using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaAPIController(IVillaRepository villaRepository, IMapper mapper)
        {
            _villaRepository = villaRepository;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetVillasAsync()
        {
            try
            {
                IEnumerable<Villa> villaList = await _villaRepository.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Villa villa = await _villaRepository.GetAsync(x => x.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaAsync([FromBody] VillaCreateDTO createDTO)
        {
            try
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
                    ModelState.AddModelError("ErrorMessages", "Villa already Exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Villa villa = _mapper.Map<Villa>(createDTO);
                villa.CreatedDate = DateTime.Now;

                await _villaRepository.CreateAsync(villa);

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaAsync(int id)
        {
            try
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

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaAsync(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                var model = _mapper.Map<Villa>(updateDTO);

                await _villaRepository.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPatch("{id}", Name = "UpdatePartialVilla")]
        [Authorize(Roles = "admin")]
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
