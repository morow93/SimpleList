using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ElasticsearchTest.Services;

namespace ElasticsearchTest.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private CardsService _service;
        public ValuesController()
        {
            _service = new CardsService();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Card> Get()
        {
            return _service.Cards();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Card Get(int id)
        {
            return _service.Get(id);
        }

        // POST api/values/add
        [HttpPost("add")]
        public Card Add([FromBody]Card card)
        {
            return _service.Add(card);
        }

        // POST api/values/add
        [HttpGet("search")]
        public IEnumerable<Card> Search(String query)
        {
            return _service.Cards().Where(c => String.IsNullOrWhiteSpace(query) || c.Title.ToLower().StartsWith(query.ToLower()));
        }

        // POST api/values/update
        [HttpPost("update")]
        public Card Update([FromBody]Card card)
        {
            return _service.Update(card);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public Card Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
