namespace backend.super_chatbot.Results
{
    public class DefaultResults
    {
        public static IResult CreateCreatedResult() =>
            TypedResults.Created();
        public static IResult CreateInvalidResult() =>
            CreateInvalidResult("dados inválidos.");
        public static IResult CreateInvalidResult(string message) =>
            TypedResults.Json(new { Mensagem = message }, statusCode: 400);
        public static IResult CreateNotFoundResult() =>
            TypedResults.NotFound();
        public static IResult CreateOkResult() =>
            TypedResults.Ok();
        public static IResult CreateOkResultResponse<TResponse>(TResponse response) =>
            TypedResults.Ok(response);
        public static IResult CreateOkResultResponse<TResponse>(List<TResponse> response) =>
            TypedResults.Ok(response);
        public static IResult CreateOkResultEntity<TEntity>(List<TEntity> entityList) =>
            TypedResults.Ok(entityList);
        public static IResult CreateOkResultMessage(string message) =>
            TypedResults.Ok(new { Mensagem = message });
        public static IResult CreateOkResult(string message) =>
           TypedResults.Ok(message);


    }
}
