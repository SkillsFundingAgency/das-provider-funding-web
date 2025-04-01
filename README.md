## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Provider Funding Web

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/das-provider-funding-web?branchName=main)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=das-provider-funding-web&branchName=main)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/jira/software/c/projects/FLP/boards/753)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3480354918/Flexible+Payments+Models)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

The provider funding web project

## How It Works

This web app displays earnings information and allows download of csv data by making calls to the funding outer api to retrieve it.


## 🚀 Installation

### Pre-Requisites

* A clone of this repository
* A code editor that supports .Net8
* Azure Storage Emulator (Azureite)

### Config

Most of the application configuration is taken from the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config) and the default values can be used in most cases.  The config json will need to be added to the local Azure Storage instance with a a PartitionKey of LOCAL and a RowKey of SFA.DAS.ProviderFunding.Web_1.0. To run the application locally the following values need to be updated:

| Name                        | Value                                    |
| --------------------------- | ---------------------------------------- |
| DbConnectionString          | Your local DB instance connection string |

## 🔗 External Dependencies

None

## Running Locally

* Make sure Azure Storage Emulator (Azureite) is running
* Run the application
* Note various applications will need to also be running locally for the web app to function, funding outer api and earnings inner. Check the readme files of those applications for more information.