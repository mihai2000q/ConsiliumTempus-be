export default interface UpdateProjectTaskRequest {
  id: string,
  name: string,
  isCompleted: boolean,
  assigneeId?: string | null
}