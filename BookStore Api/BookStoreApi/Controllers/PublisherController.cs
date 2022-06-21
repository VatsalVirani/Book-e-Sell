using BookStoreModels.Models;
using BookStoreModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BookStoreRepository;
using System.Net;

namespace BookStoreApi.Controllers
{
    [Route("api/publisher")]
    [ApiController]
    public class PublisherController : Controller
    {
        PublisherRepository _publisherRepository = new PublisherRepository();

        [Route("list")]
        [HttpGet]
        public IActionResult GetPublishers(string? keyword, int pageIndex = 1, int pageSize = 10)
        {

            try
            {
                var publishers = _publisherRepository.GetPublishers(pageIndex, pageSize, keyword);
                if (publishers == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Please provide correct information");

                ListResponse<PublisherModel> listResponse = new ListResponse<PublisherModel>()
                {
                    Records = publishers.Records.Select(x => new PublisherModel(x)).ToList(),
                    TotalRecords = publishers.TotalRecords
                };
                return StatusCode(HttpStatusCode.OK.GetHashCode(), listResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult GetPublisher(int id)
        {
            try
            {
                var response = _publisherRepository.GetPublisher(id);
                if (response == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Please provide correct information");
                var publisher = new PublisherModel(response);
                return StatusCode(HttpStatusCode.OK.GetHashCode(), publisher);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("add")]
        [HttpPost]
        public IActionResult AddPublisher(PublisherModel publisherModel)
        {
            try
            {
                Publisher publisher = new Publisher()
                {
                    Id = publisherModel.Id,
                    Name = publisherModel.Name,
                };
                var addedPublisher = _publisherRepository.AddPublisher(publisher);
                PublisherModel publisherModel1 = new PublisherModel(addedPublisher);
                if (addedPublisher == null)
                    return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Bad Request");
                //return Ok(user);
                return StatusCode(HttpStatusCode.OK.GetHashCode(), publisherModel1);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("update")]
        [HttpPut]
        public IActionResult UpdatePublisher(PublisherModel publisherModel)
        {
            try
            {
                if (publisherModel != null)
                {
                    Publisher publisher = new Publisher()
                    {
                        Id = publisherModel.Id,
                        Name = publisherModel.Name,
                        Address = publisherModel.Address,
                        Contact = publisherModel.Contact,
                    };
                    var response = _publisherRepository.UpdatePublisher(publisher);

                    if (response != null)
                        return StatusCode(HttpStatusCode.OK.GetHashCode(), new PublisherModel(response));
                }
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Please provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IActionResult DeletePublisher(int id)
        {
            if (id == 0)
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "id is null");
            try
            {
                bool response = _publisherRepository.DeletePublisher(id);
                if (response == true)
                    return StatusCode(HttpStatusCode.OK.GetHashCode(), "Publisher Deleted Successfully");
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Please provide correct information");
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
    }
}
