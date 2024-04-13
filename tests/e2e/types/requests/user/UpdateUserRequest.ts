export default interface UpdateUserRequest {
  firstName: string,
  lastName: string,
  role?: string | null | undefined,
  dateOfBirth?: string | null | undefined,
}