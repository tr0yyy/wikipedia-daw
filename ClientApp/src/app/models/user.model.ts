export class User {
    public name: string;
    public roles: string[];
    public token: string;
  
    constructor(name: string, roles: string[], token: string) {
      this.name = name;
      this.roles = roles;
      this.token = token;
    }
  }