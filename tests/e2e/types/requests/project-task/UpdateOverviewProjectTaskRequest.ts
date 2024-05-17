export default interface UpdateOverviewProjectTaskRequest {
  id: string,
  name: string,
  description: string,
  assigneeId?: string|null
}