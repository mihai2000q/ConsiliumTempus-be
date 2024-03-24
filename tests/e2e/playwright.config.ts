import { defineConfig, devices } from '@playwright/test';
// @ts-ignore
import dotenv from 'dotenv';
// @ts-ignore
import path from 'path';

dotenv.config({ path: path.resolve(__dirname, '../..', '.env') });

export default defineConfig({
  testDir: './tests',
  fullyParallel: false,
  forbidOnly: !!process.env.CI,
  retries: 0,
  workers: 1,
  reporter: 'html',
  use: {
    baseURL: `http://127.0.0.1:${process.env.API_PORT}`,
    extraHTTPHeaders: {
      'Authorization': `token ${process.env.API_TOKEN}`
    }
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    }
  ],
});
