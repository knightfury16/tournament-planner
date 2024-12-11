# Identity Itegration

## What I want to do?
1. Add authentication to application
2. Introduce **Role** based authorization

Keep the Domain.User and ApplicationUser separate.


I will have my Application User in Infratructure Layer

- Create an ApplicationUser that inherits from IdentityUser
- Change the DataContext inheritence to IdentityDbContext<ApplicationUser>
- Register Identity with the custom Application User
    ```
        services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<TournamentPlannerDataContext>()
        .AddDefaultTokenProviders();
    ```
- Add a **Service** to map from Domain.user to ApplicationUser. In this way I dont have to change
much to the existing code. Only need to do the mapping in Infrastructure layer or application layer.
- Need to have **User Manager Service** and **SingIn Manager Service** in *Application Layer*
- During a Request I need to map between Domain.User and ApplicationUser accordingly
- Need to change the default identity config like what kind of authentication i want to do and so on.(Cookie base or token base)




use cookie base authentication