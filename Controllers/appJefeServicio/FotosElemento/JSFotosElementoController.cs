
using AWArchivos.Models.appJefeServicio.Elemento;
using Microsoft.AspNetCore.Mvc;

namespace AWArchivos.Controllers.appJefeServicio.FotosElemento
{
    [ApiController]
    public class JSFotosElementoController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public JSFotosElementoController (IWebHostEnvironment webhost)
        {
            _webHostEnvironment = webhost;
        }
        [HttpPost("appjs/elemento/upload-fotos")]
        
        public ElementoFotosModel saveFotosElemento([FromForm] string AsistenciaId ,IFormFile selfiePhoto, IFormFile bodyPhoto)
        {
            //List<ResponseModel> responses = new List<ResponseModel>();
            ElementoFotosModel response = new ElementoFotosModel();
            try
            {
                if (selfiePhoto == null || bodyPhoto == null || AsistenciaId == "")
                {
                    //ResponseModel resp = new ResponseModel();
                    response.Success = false;
                    response.Code = 400;
                    response.Mensaje = "No se puede procesar la petición porque falta información";

                    return response;
                }

                var headerPath = "C:\\inetpub\\wwwroot\\";
                var bodyPath = "Archivos\\apps\\jefeServicio\\fotosElementos\\";
                var pathG = Path.Combine(_webHostEnvironment.ContentRootPath, headerPath + bodyPath + AsistenciaId);

                if (!Directory.Exists(pathG))
                {
                    Directory.CreateDirectory(pathG);
                }
                //PARA GUARDAR LA SELFIE
                string pathselfie = Path.Combine(pathG, selfiePhoto.FileName);
                string pathToSaveInDBSelfie = Path.Combine(bodyPath + AsistenciaId, selfiePhoto.FileName);
                using (var stream = new FileStream(pathselfie, FileMode.Create))
                {
                    selfiePhoto.CopyTo(stream);
                }


                //PARA GUARDAR LA BODYFULL
                string pathBodyP = Path.Combine(pathG, bodyPhoto.FileName);
                string pathToSaveInDBBodyP = Path.Combine(bodyPath + AsistenciaId, bodyPhoto.FileName);
                using (var stream = new FileStream(pathBodyP, FileMode.Create))
                {
                    bodyPhoto.CopyTo(stream);
                }

                //CREA LA RESPONSE
                response.Success = true;
                response.Code = 200;
                response.Mensaje = "Guardado";
                response.PathSelfie = pathToSaveInDBSelfie;
                response.PathBody = pathToSaveInDBBodyP;

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
