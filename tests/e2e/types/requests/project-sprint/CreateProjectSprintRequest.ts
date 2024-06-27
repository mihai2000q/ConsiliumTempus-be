export default interface CreateProjectSprintRequest {
  projectId: string,
  name: string,
  startDate?: string | null,
  endDate?: string | null,
  keepPreviousStages?: boolean | undefined,
  projectStatus?: {
    title: string,
    status: string,
    description: string
  }
}