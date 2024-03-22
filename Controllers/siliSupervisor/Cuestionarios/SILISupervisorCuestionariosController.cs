using AWArchivos.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace AWArchivos.Controllers.siliSupervisor.Cuestionarios
{
    public class SILISupervisorCuestionariosController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SILISupervisorCuestionariosController(IWebHostEnvironment webhost)
        {
            _webHostEnvironment = webhost;
        }

        [HttpPost("silisuperv/cuestionario/upload-file-respuesta")]
        public ResponseModel saveFileRespuesta([FromForm] string cuestionarioId, string campoId, string uuid, IFormFile archivo)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                if (archivo == null || cuestionarioId == "" || campoId == "" || uuid == "") 
                {
                    response.Success = false;
                    response.Code = 400;
                    response.Mensaje = "No se puede procesar la petición porque falta información";

                    return response;
                }

                var headerPath = "C:\\inetpub\\wwwroot\\";
                var bodyPath = "Archivos\\apps\\siliSupervisor\\Cuestionarios\\";
                var pathG = Path.Combine(_webHostEnvironment.ContentRootPath, headerPath + bodyPath + cuestionarioId + "\\" + campoId + "\\" + uuid);

                if (!Directory.Exists(pathG))
                {
                    Directory.CreateDirectory(pathG);
                }

                //GUARDA EL ARCHIVO
                string patharchivo = Path.Combine(pathG, archivo.FileName);
                string pathToSaveInDBArchivo = Path.Combine(bodyPath + cuestionarioId + "\\" + campoId + "\\" + uuid, archivo.FileName);
                using (var stream = new FileStream(patharchivo, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                }

                //CREA LA RESPONSE
                response.Success = true;
                response.Code = 200;
                response.Mensaje = "Guardado";
                response.Path = pathToSaveInDBArchivo;

                //RESPONDE OK
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
