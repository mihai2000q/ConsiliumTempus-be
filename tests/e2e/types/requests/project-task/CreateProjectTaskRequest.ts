export default interface CreateProjectTaskRequest {
  projectStageId: string,
  name: string,
  onTop?: boolean | undefined
}