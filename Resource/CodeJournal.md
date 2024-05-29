# Code Journal

## Asp Middleware

- Adds to app() pipeline
- Each middleware chosse whether to pass the request to next component in pipeline
- Can perform work before and after the component in the pipeline
- When a middleware short-circuits its called a *terminal middleware*
