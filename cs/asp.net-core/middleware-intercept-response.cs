// ToDo: Outline difference between how to set response values before further processing is done and after further processing has happened but before response has been sent to client.

// ToDo: Write example of letting middleware further down the pipe do its thing first and then manipulating the response before it's being sent back to the client.
public class EverythingIsFineMiddleare
{
	private RequestDelegate _next;

	public EverythingIsFineMiddleare(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		// Set up callback that will get called once a response to the client is beginning to be sent.
		// This is the point in time that you have to hook in to if you want to manipulate for intance the headers of the request before it's sent back to the client.
		// That is because (as of this date) the response is being streamed back to the client as it is being written.
		// And because the headers are at the beginning of the response/being sent first, you have to be the first to wrtie the value if you want your value to be the one the client gets.
		context.Response.OnStarting((state) =>
		{
			context.Response.StatusCode = HttpStatusCode.OK; // Everything is fine. Always.
			return Task.CompletedTask;
		}, null);

		// Let the next middleware down the line do it's thing.
		// This gets executed before the callback above.
		await _next.Invoke(context);
	}
}

public static class EverythingIsFineMiddleareExtensions
{
	public static IApplicationBuilder UseEverythingIsFineMiddleare(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<EverythingIsFineMiddleare>();
	}
}

// ToDo: Short circit futher processing and start sending response before it happens
public class SometimesNopeMiddleware
{
	private RequestDelegate _next;
	private Random _random;

	public SometimesNopeMiddleware(RequestDelegate next)
	{
		_next = next;
		_random = new Random();
	}

	public async Task InvokeAsync(HttpContext context)
	{
		// Sometimes processing of the request stops here, and we tell the client nope.
		if(_random.next > 0.5)
		{
			context.Response.StatusCode = HttpStatusCode.ServiceUnavailable;
			await context.Response.WriteAsync("Nope!");
			return;
		}

		// Other times, we let the processing continue so that the others down the pipeline do things.
		await _next.Invoke(context);
	}
}

public static class SometimesNopeMiddlewareExtensions
{
	public static IApplicationBuilder UseSometimesNopeMiddleware(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<SometimesNopeMiddleware>();
	}
}
