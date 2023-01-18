# Web App Sandbox

A sandbox to build some web apps.

# 🥅 Main Goals For Now / Checklist:

- ✔️ Learn how to set up a client to interface with an Identity / Access Management (IAM) framework (Google, Facebook, etc...)

    - ✔️ This will produce an *ID Token* which will be passed to an *Authentication Service*

- ❌ Learn how to set up a _.NET Core API_ service that manages user data (add, delete, modify users for example).

    - ✔️ This API may have an *Authentication Controller* that serves an *Authentication Token* given an *ID Token* from an IAM.  It is also possible this should be a seperate service entirely.

    - ❌ This API will have "protected" controller endpoints requiring authentication from the granted *Authentication Tokens*.

        - For a basic example, we want to do the following:

            - 1. ❌ Sign Up / Add User
            - 2. ❌ Modify User Data
            - 3. ❌ Delete User Data
            - 4. (Bonus) Get List of Data Specific To User
                - This might include more CRUD operations / endpoints to manage some kind of list for a user


- ❌ Learn how to set up a _React_ application that interfaces with our _.NET Core API_