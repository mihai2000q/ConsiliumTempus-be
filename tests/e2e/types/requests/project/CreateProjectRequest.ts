export default interface CreateProjectRequest {
  workspaceId: string,
  name: string,
  description: string,
  isPrivate: boolean
}