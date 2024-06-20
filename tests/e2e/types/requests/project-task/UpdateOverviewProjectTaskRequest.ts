export default interface UpdateOverviewProjectTaskRequest {
  id: string,
  name: string,
  description: string,
  isCompleted: boolean,
  assigneeId: string | null
}