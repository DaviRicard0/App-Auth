interface IRegister {
  name: string;
  email: string;
  password: string;
}
export class Register implements IRegister {
  name = '';
  email = '';
  password = '';
}
