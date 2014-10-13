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
    styleCopTargetPath: process.cwd() + '/' + '<%= srcPath %>packages/BuildTools.StyleCop.4.7.49.0/tools/StyleCop.targets',
    styleCopPlusTargetPath: process.cwd() + '/' + '<%= srcPath %>packages/BuildTools.StyleCopPlus.4.7.49.4/tools',

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

    fileExists: {
      styleCop: [process.cwd() + '/<%= srcPath %>packages/BuildTools.StyleCop.4.7.49.0/tools/StyleCop.targets']
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
            CustomBeforeMicrosoftCSharpTargets: process.cwd() + '\\'+'import.xml',
            CustomAfterMicrosoftCSharpTargets: '<%= styleCopTargetPath %>'
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

  // Check that stylecop exists
  grunt.registerTask('styleCopExists',function () {
    var fs = require('fs');
    if(!fs.existsSync(grunt.config().styleCopTargetPath)){
      grunt.fatal('It appears stylecop doesn\'t exist. Check that you added BuildTools.StyleCopPlus to the nuget dependencies for at least one project, and that the version number is correct, otherwise change the variable styleCopTargetPath in the gruntfile.js to reflect the right version')
    }
  })


  grunt.registerTask('default', ['build']);
  grunt.registerTask('build', ['msbuild:release']);
  grunt.registerTask('test', ['styleCopExists','msbuild:debug', 'mstest']);
  grunt.registerTask('release', ['assemblyinfo', 'test']);
}
