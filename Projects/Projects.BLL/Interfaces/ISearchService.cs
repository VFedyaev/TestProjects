using Projects.BLL.DTO;

namespace Projects.BLL.Interfaces
{
    public interface ISearchService
    {
        ModelAndViewDTO GetFilteredModelAndView(string input, string type);
    }
}
