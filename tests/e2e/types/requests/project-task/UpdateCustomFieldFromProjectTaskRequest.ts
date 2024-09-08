export default interface UpdateCustomFieldFromProjectTaskRequest {
  id: string,
  customFieldId: string,
  type: string,
  dateCustomField?: DateCustomField,
  dateTimeCustomField?: DateTimeCustomField,
  durationCustomField?: DurationCustomField,
  multiSelectCustomField?: MultiSelectCustomField,
  numberCustomField?: NumberCustomField,
  peopleCustomField?: PeopleCustomField,
  singleSelectCustomField?: SingleSelectCustomField,
  textCustomField?: TextCustomField,
  timeCustomField?: TimeCustomField,
}

interface DateCustomField {
  date?: string
}

interface DateTimeCustomField {
  dateTime?: string
}

interface DurationCustomField {
  duration?: string
}

interface MultiSelectCustomField {
  optionId: string,
  remove: boolean
}

interface NumberCustomField {
  number?: number
}

interface PeopleCustomField {
  personId?: string
}

interface SingleSelectCustomField {
  optionId?: string,
}

interface TextCustomField {
  text?: string
}

interface TimeCustomField {
  time?: string
}