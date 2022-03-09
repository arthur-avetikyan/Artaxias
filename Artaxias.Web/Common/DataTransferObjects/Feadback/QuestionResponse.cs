namespace Artaxias.Web.Common.DataTransferObjects.Feadback
{
    public class QuestionResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public AnswerResponse Answer { get; set; }
    }
}
