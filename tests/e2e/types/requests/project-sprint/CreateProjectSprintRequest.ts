export default interface CreateProjectSprintRequest {
  projectId: string,
  name: string,
  startDate: string|null,
  endDate: string|null,
}