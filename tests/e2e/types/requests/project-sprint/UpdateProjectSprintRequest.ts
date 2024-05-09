export default interface UpdateProjectSprintRequest {
  id: string,
  name: string,
  startDate: string|null,
  endDate: string|null,
}