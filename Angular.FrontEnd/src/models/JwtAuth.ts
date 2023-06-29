interface IJwtAuth {
  token: string;
  result: boolean;
  errors: string[] | null;
}
export class JwtAuth implements IJwtAuth {
  token = '';
  result = true;
  errors = [];
}
