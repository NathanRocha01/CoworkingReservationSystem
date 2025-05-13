public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public int? Id { get; }

    protected Result(bool success, string error, int? id = null)
    {
        IsSuccess = success;
        Error = error;
        Id = id;
    }

    public static Result Success(int? id = null) => new Result(true, null, id);
    public static Result Failure(string error) => new Result(false, error);
}

public class Result<T> : Result
{
    public T Data { get; }

    protected Result(bool success, T data, string error) : base(success, error)
    {
        Data = data;
    }

    public static Result<T> Success(T data) => new Result<T>(true, data, null);
    public new static Result<T> Failure(string error) => new Result<T>(false, default, error);
}