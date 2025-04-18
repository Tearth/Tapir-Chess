export class ERRORS {
  static get(key: string) {
    switch (key) {
      case 'EmptyField':
        return 'Field cannot be empty.'
      case 'InvalidUsernameOrPassword':
        return 'Invalid username or password.'
      default:
        return 'Unknown error.'
    }
  }
}
