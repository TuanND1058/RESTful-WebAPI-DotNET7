using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VillaAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillasAsync()
        {
            return Ok(await _context.Villas.ToListAsync());
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

            var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVillaAsync([FromBody] VillaCreateDTO villaDTO)
        {
            /// neu mot controller duoc danh dau [ApiController]
            /// khong can kiem tra ModelState.IsValid o day
            /// ASP.NET da tu dong kiem tra va tra ve phan hoi BadRequest neu mo hinh khong hop le
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (await _context.Villas.FirstOrDefaultAsync(x => x.Name == villaDTO.Name) != null)
            {
                ModelState.AddModelError("CustomErrorName", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            //if (villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            var model = new Villa()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                CreatedDate = DateTime.Now
            };

            await _context.Villas.AddAsync(model);
            await _context.SaveChangesAsync();

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

            var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVillaAsync(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            //villa.Name = villaDTO.Name;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Sqft = villaDTO.Sqft;

            var model = new Villa()
            {
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                UpdateDate = DateTime.Now
            };

            _context.Villas.Update(model);
            await _context.SaveChangesAsync();

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

            var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return BadRequest();
            }

            var villaDTO = new VillaUpdateDTO()
            {
                Id = villa.Id,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new Villa()
            {
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                UpdateDate = DateTime.Now
            };

            _context.Villas.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
