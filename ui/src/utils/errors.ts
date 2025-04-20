export class ERRORS {
  static get(key: string) {
    switch (key) {
      case 'EmptyField':
        return 'Field cannot be empty.'
      case 'InvalidUsernameOrPassword':
        return 'Invalid username or password.'
      case 'InvalidEmail':
        return 'Invalid e-mail address.'
      case 'PasswordTooShort':
        return 'Password is too short.'
      case 'PasswordRequiresNonAlphanumeric':
        return 'Password requires non alphanumeric.'
      case 'PasswordRequiresDigit':
        return 'Password requires digit.'
      case 'PasswordRequiresUpper':
        return 'Password requires uppercase char.'
      case 'PasswordRequiresLower':
        return 'Password requires lowercase char.'
      case 'DuplicateEmail':
        return 'Duplicated e-mail address.'
      case 'DuplicateUserName':
        return 'Username already exists.'
      case 'UserNotFound':
        return 'User does not exists.'
      default:
        return 'Unknown error.'
    }
  }
}
