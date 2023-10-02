using AWArchivos.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace AWArchivos.Controllers.appJefeServicio.fatiga
{
    [ApiController]
    public class JSFatigaController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public JSFatigaController(IWebHostEnvironment webhost)
        {
            _webHostEnvironment = webhost;
        }

        [HttpPost("appjs/fatiga/upload-file")]
        public ResponseModel saveFatigaFile(IFormFile fatiga)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (fatiga == null) 
                {
                    response.Success = false;
                    response.Code = 400;
                    response.Mensaje = "No se encontró ningun archivo adjunto";
                    return response;
                }

                var headerPath = "C:\\inetpub\\wwwroot\\";
                //var bodyPath = "Archivos\\intra\\admin\\avisos\\";//EXAMPLE
                var bodyPath = "solicitud\\Evidencias\\";//PRODUCCION
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, headerPath+bodyPath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fullPath = Path.Combine(path, fatiga.FileName);
                string pathToSaveInDB = Path.Combine(bodyPath, fatiga.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    fatiga.CopyTo(stream);
                }
                response.Success = true;
                response.Code = 200;
                response.Mensaje = "Guardado";
                response.Path = pathToSaveInDB;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Code = 400;
                response.Mensaje = ex.Message;
                return response;
            }
        }
    }
}
