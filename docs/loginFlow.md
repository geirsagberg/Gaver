# Login flow

- Request authenticated resource
- Redirected to Auth0 login
- Auth0 login successful -> (idToken, accessToken)
- GetUserInfo (accessToken)
- GetUserInfo successful -> (UserId, Name)

# Shared list flow
- Try access list
- Login successful
- API.checkAccess ->
  - if owner -> denied
  - if not invited -> denied
  - else -> allowed
