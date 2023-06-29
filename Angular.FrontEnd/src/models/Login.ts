interface ILogin {
  email: string;
  password: string;
}
export class Login implements ILogin {
  email = '';
  password = '';
}
