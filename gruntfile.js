'use strict';

module.exports = function(grunt) {

  // Load grunt tasks automatically
  require('load-grunt-tasks')(grunt);

  // Time how long tasks take. Can help when optimizing build times
  require('time-grunt')(grunt);

  grunt.initConfig({

    // Set this variables for different projects
    srcPath: 'h-dependency/',
    productName: 'h-dependency',

    // These variables shouldn't be changed, but sometimes it might be necessary
    solutionName: '<%= productName %>.sln',
    dotNetVersion: '4.5.0',
    platform: 'Any CPU',
    styleCopRules: 'Settings.StyleCop',
    ruleSet: 'rules.ruleset',

    pkg: grunt.file.readJSON('package.json'),

    assemblyinfo: {
      options: {
        files: ['<%= srcPath %><%= solutionName %>'],
        info: {
          version: '<%= pkg.version %>',
          fileVersion: '<%= pkg.version %>',
          company: 'hylasoft',
          copyright: ' ',
          product: '<%= productName %>',
          title: '<%= productName %>'
        }
      }
    },

    msbuild: {
      release: {
        src: ['<%= srcPath %><%= solutionName %>'],
        options: {
          projectConfiguration: 'Release',
          platform: '<%= platform %>',
          targets: ['Clean', 'Rebuild']
        }
      },
      debug: {
        src: ['<%= srcPath %><%= solutionName %>'],
        options: {
          projectConfiguration: 'Debug',
          platform: '<%= platform %>',
          targets: ['Clean', 'Rebuild'],
          buildParameters: {
            StyleCopEnabled: true,
            StyleCopTreatErrorsAsWarnings: false,
            StyleCopOverrideSettingsFile: '../<%= styleCopRules %>',
            RunCodeAnalysis: true,
            CodeAnalysisRuleSet: '<%= ruleSet %>',
            TreatWarningsAsErrors: true
          },
        }
      }
    },

    mstest: {
      debug: {
        src: ['<%= srcPath %>/**/bin/Debug/*.dll'] // Points to test dll
      }
    }

  });

  grunt.registerTask('default', ['build']);
  grunt.registerTask('build', ['msbuild:release']);
  grunt.registerTask('test', ['msbuild:debug', 'mstest']);
  grunt.registerTask('release', ['test', 'assemblyinfo']);
}
