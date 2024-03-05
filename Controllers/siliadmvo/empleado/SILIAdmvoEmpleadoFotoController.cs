using AWArchivos.Models.siliadmvo.empleado;
using Microsoft.AspNetCore.Mvc;

namespace AWArchivos.Controllers.siliadmvo.empleado
{
    public class SILIAdmvoEmpleadoFotoController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SILIAdmvoEmpleadoFotoController(IWebHostEnvironment webhost)
        {
            _webHostEnvironment = webhost;
        }

        [HttpPost("siliadmvo/empleado/upload-foto")]
        public EmpleadoFotoModel saveFotoEmpleado([FromForm] string AsistenciaId, IFormFile selfiePhoto)
        {
            EmpleadoFotoModel response = new EmpleadoFotoModel();

            try
            {
                if (selfiePhoto == null || AsistenciaId == "")
                {
                    response.Success = false;
                    response.Code = 400;
                    response.Mensaje = "No se puede procesar la petición porque falta información";

                    return response;
                }

                var headerPath = "C:\\inetpub\\wwwroot\\";
                var bodyPath = "Archivos\\apps\\siliAdmvo\\fotosEmpleados\\";
                var pathG = Path.Combine(_webHostEnvironment.ContentRootPath, headerPath + bodyPath + AsistenciaId);
                
                if (!Directory.Exists(pathG))
                {
                    Directory.CreateDirectory(pathG);
                }
                //GUARDA LA SELFIE
                string pathselfie = Path.Combine(pathG, selfiePhoto.FileName);
                string pathToSaveInDBSelfie = Path.Combine(bodyPath + AsistenciaId, selfiePhoto.FileName);
                using (var stream = new FileStream(pathselfie, FileMode.Create))
                {
                    selfiePhoto.CopyTo(stream);
                }

                //CREA LA RESPONSE
                response.Success = true;
                response.Code = 200;
                response.Mensaje = "Guardado";
                response.PathSelfie = pathToSaveInDBSelfie;

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
