# Code Journal

## Asp Middleware

- Adds to app() pipeline
- Each middleware chosse whether to pass the request to next component in pipeline
- Can perform work before and after the component in the pipeline
- When a middleware short-circuits its called a _terminal middleware_

So we can configure middleware in **3 different ways**.

## 1. In stand alone `middleware class`.

The middleware class must include:

- A public constructor with a parameter of type `RequestDelegate`.
- A public method named `Invoke` or `InvokeAsync`. This method must: Return a Task.
- Accept a first parameter of type `HttpContext`.

Then injecting it in application pipeline through `UseMiddeware` like,

`app.UseMiddleware<ApplicationMiddleware>();`

We can also use extension method to expose the middleware class through **`IApplicationBuilder`**

Example code,

```c#
public static class RequestCultureMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestCulture(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestCultureMiddleware>();
    }
}
```

Then use it in `program.cs` like

```c#
app.UseRequestCulture();
```

## 2. Using `Use`

```c#
app.Use(async (context, next) =>{
    await context.Response.WriteAsync("A: Before\n");
    await next.Invoke();
    await context.Response.WriteAsync("A: After\n");
});
```

The `next` parameter represent the next delegate in the pipeline. I can short circuit the pipeline by not calling the `next`
I can perform _action_ both **before** and **after** invoking `next()`.

Like here, lets say after `next` it goes to a controller and perform its logic.After that it will return to this middleware and execute the remaining lines afte `next`

## 3. Using `Run`

Run delegates don't receive a next parameter. The first Run delegate is always terminal and terminates the pipeline. Run is a convention. Some middleware components may expose Run[Middleware] methods that run at the end of the pipeline:

```c#
app.Run(
    async (context) =>{
       await context.Response.WriteAsync("From Terminal middleware: returning...\n");
    }
);

app.Run();
```

If middleware is used in `Run` it just terminate the request after running it. Does not even go to controller.

## Experiment 1.

```c#
app.Use(async (context, next) =>{
    await context.Response.WriteAsync("A: Before\n");
    await next.Invoke();
    await context.Response.WriteAsync("A: After\n");
});

app.MapGet("/hello", () => {
    return "hello world";
});

app.Run(
    async (context) =>{
       await context.Response.WriteAsync("From Terminal middleware: returning...\n");
    }
);
app.Run();
```

This program output

```
A: Before
From Terminal middleware: returning...
A: After
```

Notice it does not hit the controller. Also it return to the first middleware and execute the lines after `next` invocation.


## Experiment 2.

Consider the following code

```c#
app.Use(async (context, next) =>{
    await context.Response.WriteAsync("A: Before\n");
    await next.Invoke();
    await context.Response.WriteAsync("A: After\n");
});

app.MapGet("/hello", () => {
    return  "hello world";
});
```

On execution of the following code I get the following error.
**`Headers are read-only, response has already started`**


Occurs when an attempt is made to modify the response headers after the response body has already begun to be sent to the client. Like,
```c#
await context.Response.WriteAsync("A: Before\n");
```
By default this set the context header as **`application/octet-stream`**

> **application/octet-stream** is a generic content type used for binary data or data streams where the specific content type is unknown.

Then controller try to write **`plain/text`** to the response.

```c#
app.MapGet("/hello", () => {
    return  "hello world";
});
```
So there arise a conflict.

To solve this error we can explicitly set header before any response is being sent.

```c#
app.Use(async (context, next) =>
{
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync("A: Before\n");
    await next.Invoke();
    await context.Response.WriteAsync("A: After\n");
});

app.MapGet("/hello", () =>
{
    return "hello world\n";
});
```
This output 

```
A: Before
hello world
A: After
```


## Experiment 3.
Consider the following code

```c#
app.Use(async (context, next) =>
{
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync("A: Before\n");
    await next.Invoke();
    await context.Response.WriteAsync("A: After\n");
});

app.Use(async (context, next) =>{
    await context.Response.WriteAsync("B: Before\n");
    await next.Invoke();
    await context.Response.WriteAsync("B: After\n");
});

app.MapGet("/hello", () =>
{
    return "hello world\n";
});
```

this output 

```
A: Before
B: Before
hello world
B: After
A: After
```
The flow will be,

```mermaid
flowchart LR
    A(Request) -->|1| B(A Middleware)
    B -->|2| C(B Middleware)
    C -->|3| D('/hello' controller)
    D --> |4 resposne| C
    C --> |5| B
```   

## Experiment 4.

Consider the following code

```c#

app.Use(async (context, next) =>
{
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync("A: Before\n");
    await next.Invoke();
    await context.Response.WriteAsync("A: After\n");
});

app.Use(async (context, next) =>{
    await context.Response.WriteAsync("B: Before\n");
    await next.Invoke();
    await context.Response.WriteAsync("B: After\n");
});

app.MapGet("/hello", () =>
{
    return "hello world\n";
});

app.Run(
    async (context) =>{
       await context.Response.WriteAsync("From Terminal middleware: returning...\n");
    }
);
app.Run();
```

this output

```
A: Before
B: Before
From Terminal middleware: returning...
B: After
A: After
```

Notice it does not even go to the controller. It hits a `terminal` middleware and short circuit there. And the next it just propagte to the middleware and execute the lines after `next`






### No Net(19/07/24)
Its the second day without internet. Tried to add the CreatedAt and UpdatedAt auto property of the BaseEntity which i already did. But forgot to pull it. Now without net I am unable to do it again. So much code is dependent on Internet.

Without this cant seem to do anything. Well maybe I can so something in the frontend. Well maybe tomorrow.

### No Net(20/07/24)
First day of Curfew. They say they are gonna lift it tommorrow. Lets hope for best.

### No Net(21/07/24)
Finally remembered the forgotten code. Added the timeStamp on all entity on model Creation.
Now need to remember the OnSave ovverride method. Damn it.

### No Net(22/07/24)
Could not remember the AddTimeStamp() method. Need internet.