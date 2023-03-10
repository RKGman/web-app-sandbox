# Web App Sandbox

A sandbox to build some web apps.

# ð¥ Main Goals For Now / Checklist:

- âï¸ Learn how to set up a client to interface with an Identity / Access Management (IAM) framework (Google, Facebook, etc...)

    - âï¸ This will produce an *ID Token* which will be passed to an *Authentication Service*

- âï¸ Learn how to set up a _.NET Core API_ service that manages user data (add, delete, modify users for example).

    - âï¸ This API will have an *Authentication Controller* that serves an *Authentication Token* given an *ID Token* from an IAM.  It is also possible this should be a seperate service entirely.

    - âï¸ This API will have "protected" controller endpoints requiring authentication from the granted *Authentication Tokens*.

        - For a basic example, we want to do the following:

            - âï¸Sign Up / Add User
            - âï¸ Modify User Data
            - âï¸ Delete User Data
            - âï¸ Prevent users from being able to modify or access other user data. 
            - âï¸ (Bonus) Get List of Data Specific To User
                - Not sure, if a "Login" method is required on the API, or if AuthTokens suffice and client manages that "state"?
                - This might include more CRUD operations / endpoints to manage some kind of list for a user
            - â (Bonus) Basic unit tests to gate a deployment in the pipeline.         
                

- âï¸ Create React App 
    - â Figure out how to implement Google Authentication
        - âï¸ Figure out how to implement login from Google
        - â Figure out how to implement logout from Google
        - â Get React routing working
    - â Learn how to set up a _React_ application that interfaces with our _.NET Core API_
        - â Set up API calls to .Net API
    - â Create components for basic list functionality
    - â Create components related to user profile (modifying user data)