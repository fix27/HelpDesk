using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.DataService.Specification;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для запроса данных из справочника должностей
    /// </summary>
    public class PostService : BaseService, IPostService
    {
        
        private readonly IBaseRepository<Post> postRepository;
        
        public PostService(IBaseRepository<Post> postRepository)
        {
            this.postRepository = postRepository;
        }
        
        public IEnumerable<Post> GetList(string name)
        {
            return postRepository
                .GetList(new SimpleEntityByNameLikeSpecification<Post>(name))
                .ToList();
        }
               
    }
}
