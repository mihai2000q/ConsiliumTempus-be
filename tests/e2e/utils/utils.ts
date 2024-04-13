export function useToken(
  token: string | undefined = process.env.API_TOKEN
) {
  return {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  }
}