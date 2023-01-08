export interface Result<TResponse> {
    isSuccess: boolean;
    result: TResponse;
    errors: string;
  }