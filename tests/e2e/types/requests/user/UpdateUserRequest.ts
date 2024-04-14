export default interface UpdateUserRequest {
  firstName: string,
  lastName: string,
  role: string | null,
  dateOfBirth: string | null,
}