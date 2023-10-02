using AWArchivos.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace AWArchivos.Controllers.Intra.Admin.Avisos
{
    [ApiController]
    public class AvisosController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AvisosController(IWebHostEnvironment webhost)
        {
            _webHostEnvironment = webhost;
        }

        [HttpGet("prueba")]
        public string Prueba()
        {
            return "La Api funciona correctamente";
        }


        [HttpPost("intra/admin/notices/upload-file")]
        //public ResponseModel SaveFile (IFormFile aviso, [FromBody] AvisoModel notice)
        public ResponseModel SaveFile (IFormFile aviso,[FromForm] string AvisoId)
        {
            ResponseModel response = new ResponseModel();

            try { 
                if (aviso == null)
                {
                    response.Success = false;
                    response.Code = 400;
                    response.Mensaje = "No se encontró ningun archivo adjunto.";
                    return response;
                }
                //var path = Path.Combine(_webHostEnvironment.ContentRootPath, "C:\\archivos\\intra\\admin\\avisos\\"+AvisoId);
                var headerPath = "C:\\inetpub\\wwwroot";
                var bodyPath = "\\Archivos\\intra\\admin\\avisos\\";
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, headerPath + bodyPath+ AvisoId);
                //\\192.168.11.75\c$\inetpub\wwwroot\APIS\Archivos\intra\admin\avisos


                if (!Directory.Exists(path)) 
                { 
                    Directory.CreateDirectory(path);
                }

                string fullPath = Path.Combine(path, aviso.FileName);
                string pathToSaveInDB = Path.Combine(bodyPath+AvisoId, aviso.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    aviso.CopyTo(stream);
                }
                response.Success = true;
                response.Code = 200;
                response.Mensaje = "Guardado";
                response.Path = pathToSaveInDB;
                return response;
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Code = 400;
                response.Mensaje = ex.Message;
                return response;
            }
        }

    }
}
