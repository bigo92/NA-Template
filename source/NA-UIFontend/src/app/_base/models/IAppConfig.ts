export interface IAppConfig {
  env: {
    name: string;
    virtual: boolean;
    prod: boolean;
  };

  apiServer: {
    physical: string;
    virtual: string;
  };
}
