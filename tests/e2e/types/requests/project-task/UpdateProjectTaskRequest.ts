export default interface UpdateProjectTaskRequest {
  id: string,
  name: string,
  assigneeId?: string | null
}