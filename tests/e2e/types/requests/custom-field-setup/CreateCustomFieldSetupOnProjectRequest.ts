export default interface CreateCustomFieldSetupOnProjectRequest {
  projectId: string,
  name: string,
  description: string,
  type: string,
  numberSettings?: NumberSettings | undefined,
  singleSelectOptions?: SingleSelectOption[] | undefined
}

interface NumberSettings {
  currencyCode: string,
  decimals: number,
  rounding: boolean,
}

interface SingleSelectOption {
  value: string,
  color: string
}